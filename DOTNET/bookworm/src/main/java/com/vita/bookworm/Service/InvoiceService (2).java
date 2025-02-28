package com.vita.bookworm.Service;

import java.time.LocalDate;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.vita.Models.CartDetails;
import com.vita.Models.CartMaster;
import com.vita.Models.CustomerMaster;
import com.vita.Models.Invoice;
import com.vita.Models.InvoiceDetails;
import com.vita.Models.ProductBeneficiary;
import com.vita.Models.RoyaltyCalculation;
import com.vita.bookworm.Repository.InvoiceDetailsRepository;
import com.vita.bookworm.Repository.InvoiceRepository;
import com.vita.bookworm.Repository.ProductBeneficiaryRepository;
import com.vita.bookworm.Repository.RoyalityCalculationRepository;

import jakarta.transaction.Transactional;

@Service
public class InvoiceService {
    @Autowired
    private InvoiceRepository invoiceRepository;

    @Autowired
    private CartServices cartServices;

    @Autowired
    private CustomerMasterServices customerMasterServices;

    @Autowired
    private InvoiceDetailsRepository invoiceDetailsRepository;

    @Autowired
    private CartDetailsService cartDetailsService;

    @Autowired
    private ProductBeneficiaryRepository productBeneficiaryRepository;

    @Autowired
    private RoyalityCalculationRepository royalityCalculationRepository;

    @Transactional
    public void generateInvoice(int custId) {
        Integer cartId = cartServices.getActiveCart(custId);
        if (cartId ==null) return;

        CartMaster cm = cartServices.getCart(cartId);
        if (cm == null || !cm.getIsActive()) {
            return;
        }

        List<CartDetails> cd = cartDetailsService.findByCartId(cartId);
        if (cd == null || cd.isEmpty() ){
            return;
        }
        Invoice inv = new Invoice();
        inv = invoiceRepository.save(inv);
        inv.setAmount(cm.getCost());
        inv.setCartId(cm);
        inv.setCustomerId(customerMasterServices.getCustomerMasterById(custId));
        inv.setDate(LocalDate.now());
        for (CartDetails c : cd ) {
            InvoiceDetails id = new InvoiceDetails();
            id.setInvoice(inv);
            id.setProduct(c.getProductId());
            id.setTranType(c.getIsRented() ? "r" : "p");
            id.setRentNoOfDays(c.getRentNoOfDays());
            id.setSalePrice(c.getProductId().getProductSpCost());
            id.setBasePrice(c.getProductId().getProductBasePrice());
            invoiceDetailsRepository.save(id);
        }
    }

    @Transactional
    public void afterCheckOut(int custId) {
        CustomerMaster cm = customerMasterServices.getCustomerMasterById(custId);
        if (cm == null) return;
        Integer cid = cartServices.getActiveCart(custId);
        if (cid == null) return;
        CartMaster cam = cartServices.getCart(cid);
        cam.setIsActive(false);
        CartMaster tcam = new CartMaster();
        tcam.setCost(0.);
        tcam.setCustomerId(cm);
        tcam.setIsActive(true);
        cartServices.addCart(tcam);

    }

    @Transactional
    public void addRoyaltyCalculation(Integer custId) {
        if (custId == null) return;
        Integer i = cartServices.getActiveCart(custId);
        if (i == null) return;
        List<CartDetails> cds = cartServices.getDetails(custId);
        if (cds == null || cds.isEmpty()) return;
        for (CartDetails cd : cds) {
            RoyaltyCalculation temp = new RoyaltyCalculation();
            temp.setBasePrice(cd.getProductId().getProductBasePrice());
            ProductBeneficiary bm = productBeneficiaryRepository.getByProductId(cd.getProductId().getProductId()).get(0);
            if (bm == null) return;
            temp.setBeneficiaryMaster(bm.getBeneficiaryMaster());
            temp.setProduct(cd.getProductId());
            temp.setTransactionType(cd.getIsRented() ? "R" : "P");
            temp.setRoyaltyTranDate(LocalDate.now());
            if (cd.getIsRented() && cd.getProductId().getIsRentable()){
                temp.setBasePrice(cd.getProductId().getRentPerDay());
                temp.setRoyaltyOnBasePrice((cd.getProductId().getRentPerDay() * cd.getRentNoOfDays())/(bm.getPercentage()));
                temp.setSalesPrice(temp.getRoyaltyOnBasePrice());
            }else{
                temp.setBasePrice(cd.getProductId().getProductBasePrice());
                temp.setRoyaltyOnBasePrice((cd.getProductId().getProductBasePrice() ) / bm.getPercentage());
                temp.setSalesPrice((cd.getProductId().getProductBasePrice() - (cd.getProductId().getProductOfferPrice() == null ? 0 : cd.getProductId().getProductOfferPrice() )) / bm.getPercentage());
            }
            royalityCalculationRepository.save(temp);
        }
    }
}
