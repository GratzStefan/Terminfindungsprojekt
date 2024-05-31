package com.example.demo.services;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.dtos.UserDTO;
import org.bson.types.ObjectId;

import java.util.List;

public interface OrganizationService {
    ObjectId create(OrganizationDTO OrganizationDTO);
    List<OrganizationDTO> search(String organizationName);
    List<OrganizationDTO> searchOrganizations(String userid);
    List<UserDTO> userListOfOrganization(String orgid);
    boolean promoteUser(String orgid, String userid, String admin);
    long delete(String id);
    boolean removeUser(String userid, String orgid, String adminid);
}
