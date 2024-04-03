package com.example.demo.repositories;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.models.OrganizationEntity;
import org.bson.types.ObjectId;

import java.util.List;

public interface OrganizationRepository {
    ObjectId create(OrganizationEntity organizationEntity);
    int addUser(String userid, String organizationid, String adminid);
    List<OrganizationEntity> search(String organizationName);
    List<OrganizationEntity> searchOrganization(String userid);
    OrganizationEntity modify(OrganizationEntity organizationEntity);
    long delete(String id);
}
