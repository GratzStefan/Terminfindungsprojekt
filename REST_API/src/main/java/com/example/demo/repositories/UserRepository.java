package com.example.demo.repositories;

import com.example.demo.dtos.UserDTO;
import com.example.demo.models.UserEntity;
import org.springframework.stereotype.Repository;


@Repository
public interface UserRepository {
    UserEntity register(UserEntity userEntity);
    UserDTO login(String username, String password);
    long update(UserEntity userEntity);
    long delete(String id);
}
