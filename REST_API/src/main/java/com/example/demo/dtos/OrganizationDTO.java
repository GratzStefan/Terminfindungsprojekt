package com.example.demo.dtos;

import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.OrganizationRole;
import com.example.demo.models.UserEntity;
import com.fasterxml.jackson.annotation.JsonInclude;
import org.bson.types.ObjectId;

import java.util.HashMap;
@JsonInclude(JsonInclude.Include.NON_NULL)
public record OrganizationDTO (
        String id,
        String name,
        String creatorid) {

    public OrganizationDTO(OrganizationEntity o){
        this(o.getId() == null ? new ObjectId().toHexString() : o.getId().toHexString(), o.getName(), o.getCreatorid());
    }

    public OrganizationEntity toOrganizationEntity(){
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        if(creatorid != null){
            HashMap<String, Integer> hashMap = new HashMap<String, Integer>();
            hashMap.put(creatorid, OrganizationRole.Admin.ordinal());
            return new OrganizationEntity(_id, name, hashMap);
        }

        return new OrganizationEntity(_id, name);
    }
}
