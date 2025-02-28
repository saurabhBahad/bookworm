package com.vita.bookworm.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.vita.Models.BeneficiaryMaster;

@Repository
public interface BeneficiaryRepository extends JpaRepository<BeneficiaryMaster , Integer>{
}
