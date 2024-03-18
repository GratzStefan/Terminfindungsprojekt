package com.example.demo.repositories;

import com.example.demo.models.UserEntity;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;

@Repository
public interface UserRepository {
    UserEntity register(UserEntity userEntity);
    ObjectId login(String username, String password);
    UserEntity update(UserEntity userEntity);
    long delete(String id);
}
