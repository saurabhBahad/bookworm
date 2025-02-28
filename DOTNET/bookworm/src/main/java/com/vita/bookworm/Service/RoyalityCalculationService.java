package com.vita.bookworm.Service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.vita.Models.RoyaltyCalculation;
import com.vita.bookworm.Repository.RoyalityCalculationRepository;

@Service
public class RoyalityCalculationService {
    @Autowired
    private RoyalityCalculationRepository royalityCalculationRepository;

    public RoyaltyCalculation addRoyalitCalculation(RoyaltyCalculation r) {
        if (r == null) return r;
        return royalityCalculationRepository.save(r);
    }
}
