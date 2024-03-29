package com.example.demo.services;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.dtos.UserDTO;
import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.UserEntity;
import com.example.demo.repositories.OrganizationRepository;
import com.example.demo.repositories.UserRepository;
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
    public ObjectId addUser(String userid, String organizationid, String adminid) {
        return organizationRepository.addUser(userid, organizationid, adminid);
    }

    @Override
    public List<OrganizationDTO> search(String organizationName) {
        return organizationRepository.search(organizationName).stream().map(OrganizationDTO::new).toList();
    }

    @Override
    public OrganizationDTO modify(OrganizationDTO OrganizationDTO) {
        return new OrganizationDTO(organizationRepository.modify(OrganizationDTO.toOrganizationEntity()));
    }

    @Override
    public long delete(String id) {
        return organizationRepository.delete(id);
    }
}
