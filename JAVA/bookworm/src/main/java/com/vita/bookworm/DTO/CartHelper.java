package com.vita.bookworm.DTO;

import lombok.Data;

@Data
public class CartHelper {
        private Integer CustId;
        private Integer ProdId;
        private boolean isRented;
        private Integer noOfDays;
}
