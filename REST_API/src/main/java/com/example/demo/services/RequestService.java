package com.example.demo.services;

import com.example.demo.dtos.RequestDTO;

import java.util.List;

public interface RequestService {
    RequestDTO create(RequestDTO RequestDTO);
    List<RequestDTO> findToOrganization(String orgid);
    List<RequestDTO> findOfUser(String userid);
    long changeStatus(String adminid, RequestDTO RequestDTO);
}
