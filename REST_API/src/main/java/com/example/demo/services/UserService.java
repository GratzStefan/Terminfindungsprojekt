package com.example.demo.services;

import com.example.demo.dtos.UserDTO;
import org.bson.types.ObjectId;

public interface UserService {
    UserDTO register(UserDTO UserDTO);
    ObjectId login(String username, String password);
    UserDTO update(UserDTO UserDTO);
    long delete(String id);
}
