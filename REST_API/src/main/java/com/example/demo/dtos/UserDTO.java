package com.example.demo.dtos;

import com.example.demo.models.UserEntity;
import com.fasterxml.jackson.annotation.JsonInclude;
import org.bson.types.ObjectId;
@JsonInclude(JsonInclude.Include.NON_NULL)
public record UserDTO (
    String id,
    String firstname,
    String lastname,
    String username,
    String password) {

    public UserDTO (UserEntity u){
        this(u.getId() == null ? new ObjectId().toHexString() : u.getId().toHexString(), u.getFirstname(), u.getLastname(), u.getUsername(), u.getPassword());
    }

    public UserEntity toUserEntity(){
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new UserEntity(_id, firstname, lastname, username, password);
    }
}
