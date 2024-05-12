package com.example.demo.services;

import com.example.demo.dtos.UserDTO;
import com.example.demo.models.UserEntity;
import com.example.demo.repositories.UserRepository;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Service;

@Service
public class UserServiceImpl implements UserService{
    private final UserRepository userRepository;

    public UserServiceImpl(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public UserDTO register(UserDTO UserDTO) {
        UserEntity entity = userRepository.register(UserDTO.toUserEntity());

        if(entity == null) return null;

        return new UserDTO(entity);
    }

    @Override
    public UserDTO login(String username, String password) {
        return userRepository.login(username, password);
    }

    @Override
    public long update(UserDTO UserDTO) {
        return userRepository.update(UserDTO.toUserEntity());
    }

    @Override
    public long delete(String id) {
        return userRepository.delete(id);
    }
}
