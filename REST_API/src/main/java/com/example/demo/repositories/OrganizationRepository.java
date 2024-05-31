package com.example.demo.repositories;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.UserEntity;
import org.bson.types.ObjectId;

import java.util.HashMap;
import java.util.List;

public interface OrganizationRepository {
    ObjectId create(OrganizationEntity organizationEntity);
    List<OrganizationEntity> search(String organizationName);
    List<OrganizationEntity> searchOrganization(String userid);
    List<UserEntity> userListOfOrganization(String orgid);
    boolean promoteUser(String orgid, String userid, String adminid);
    long delete(String id);
    boolean removeUser(String userid, String orgid, String adminid);
}
