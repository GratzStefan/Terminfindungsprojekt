package com.example.demo.services;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.dtos.RequestDTO;
import com.example.demo.models.RequestEntity;
import com.example.demo.repositories.RequestRepository;
import com.example.demo.repositories.UserRepository;
import org.bouncycastle.cert.ocsp.Req;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class RequestServiceImpl implements RequestService {
    private final RequestRepository requestRepository;

    public RequestServiceImpl(RequestRepository requestRepository) {
        this.requestRepository = requestRepository;
    }

    @Override
    public RequestDTO create(RequestDTO RequestDTO) {
        RequestEntity ent = requestRepository.create(RequestDTO.toRequestEntity());
        if(ent==null) return null;
        return new RequestDTO(ent);
    }

    @Override
    public List<RequestDTO> findToOrganization(String orgid) {
        return requestRepository.findToOrganization(orgid).stream().map(RequestDTO::new).toList();
    }

    @Override
    public List<RequestDTO> findOfUser(String userid) {
        return requestRepository.findOfUser(userid).stream().map(RequestDTO::new).toList();
    }

    @Override
    public long changeStatus(String adminid, RequestDTO RequestDTO) {
        return requestRepository.changeStatus(adminid, RequestDTO.toRequestEntity());
    }
}
