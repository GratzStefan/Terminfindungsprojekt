package com.example.demo.dtos;

import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.UserEntity;
import org.bson.types.ObjectId;

public record OrganizationDTO (
        String id,
        String name,
        String creatorid) {

    public OrganizationDTO(OrganizationEntity o){
        this(o.getId() == null ? new ObjectId().toHexString() : o.getId().toHexString(), o.getName(), o.get);
    }

    public OrganizationEntity toOrganizationEntity(){
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new OrganizationEntity(_id, name);
    }
}
