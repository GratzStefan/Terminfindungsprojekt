package com.example.demo.dtos;

import com.example.demo.models.UserEntity;
import org.bson.types.ObjectId;

public record UserDTO (
    String id,
    String username,
    String password) {

    public UserDTO (UserEntity u){
        this(u.getId() == null ? new ObjectId().toHexString() : u.getId().toHexString(), u.getUsername(), u.getPassword());
    }

    public UserEntity toUserEntity(){
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new UserEntity(_id, username, password);
    }
}
