package com.example.demo.repositories;

import com.example.demo.models.RequestEntity;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface RequestRepository {
    RequestEntity create(RequestEntity requestEntity);
    List<RequestEntity> findToOrganization(String orgid);
    List<RequestEntity> findOfUser(String userid);
    long changeStatus(String adminid, RequestEntity requestEntity);
}
