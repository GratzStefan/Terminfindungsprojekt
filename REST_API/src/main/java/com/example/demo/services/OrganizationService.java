package com.example.demo.services;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.dtos.UserDTO;
import org.bson.types.ObjectId;

import java.util.List;

public interface OrganizationService {
    ObjectId create(OrganizationDTO OrganizationDTO);
    ObjectId addUser(String userid, String organizationid, String adminid);
    List<OrganizationDTO> search(String organizationName);
    OrganizationDTO modify(OrganizationDTO OrganizationDTO);
    long delete(String id);
}
