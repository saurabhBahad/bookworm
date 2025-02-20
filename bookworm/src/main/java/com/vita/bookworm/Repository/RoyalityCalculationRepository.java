package com.vita.bookworm.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.vita.Models.RoyaltyCalculation;

@Repository
public interface RoyalityCalculationRepository extends JpaRepository<RoyaltyCalculation, Integer>{
    
}
