package com.example.demo.services;

import com.example.demo.dtos.UserDTO;
import org.bson.types.ObjectId;

public interface UserService {
    UserDTO register(UserDTO UserDTO);
    UserDTO login(String username, String password);
    long update(UserDTO UserDTO);
    long delete(String id);
}
