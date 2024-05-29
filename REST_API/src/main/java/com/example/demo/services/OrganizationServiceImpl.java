package com.example.demo.services;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.dtos.UserDTO;
import com.example.demo.repositories.OrganizationRepository;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class OrganizationServiceImpl implements OrganizationService{
    private final OrganizationRepository organizationRepository;

    public OrganizationServiceImpl(OrganizationRepository organizationRepository) {
        this.organizationRepository = organizationRepository;
    }

    @Override
    public ObjectId create(OrganizationDTO OrganizationDTO) {
        return organizationRepository.create(OrganizationDTO.toOrganizationEntity());
    }

    @Override
    public int addUser(String userid, String organizationid, String adminid) {
        return organizationRepository.addUser(userid, organizationid, adminid);
    }

    @Override
    public List<OrganizationDTO> search(String organizationName) {
        return organizationRepository.search(organizationName).stream().map(OrganizationDTO::new).toList();
    }

    @Override
    public List<OrganizationDTO> searchOrganizations(String userid) {
        return organizationRepository.searchOrganization(userid).stream().map(OrganizationDTO::new).toList();
    }

    @Override
    public List<UserDTO> userListOfOrganization(String orgid) {
        return organizationRepository.userListOfOrganization(orgid).stream().map(UserDTO::new).toList();
    }

    @Override
    public OrganizationDTO modify(OrganizationDTO OrganizationDTO) {
        return new OrganizationDTO(organizationRepository.modify(OrganizationDTO.toOrganizationEntity()));
    }

    @Override
    public boolean promoteUser(String orgid, String userid, String adminid) {
        return organizationRepository.promoteUser(orgid, userid, adminid);
    }

    @Override
    public long delete(String id) {
        return organizationRepository.delete(id);
    }

    @Override
    public boolean removeUser(String userid, String orgid, String adminid) {
        return organizationRepository.removeUser(userid, orgid, adminid);
    }
}
