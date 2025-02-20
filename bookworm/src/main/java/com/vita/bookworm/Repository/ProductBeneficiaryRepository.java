package com.vita.bookworm.Repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.vita.Models.ProductBeneficiary;

@Repository
public interface ProductBeneficiaryRepository extends JpaRepository<ProductBeneficiary,Integer> {
    @Query(value = "select * from product_beneficiary where product_id = :prodId" , nativeQuery = true)
    List<ProductBeneficiary> getByProductId(@Param("prodId") int prodId);

    
}
