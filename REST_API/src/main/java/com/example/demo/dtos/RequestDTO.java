package com.example.demo.dtos;

import com.example.demo.models.RequestEntity;
import com.fasterxml.jackson.annotation.JsonInclude;
import org.bson.types.ObjectId;

@JsonInclude(JsonInclude.Include.NON_NULL)
public record RequestDTO (
        String id,
        UserDTO user,
        OrganizationDTO org,
        int status) {

    public RequestDTO (RequestEntity e){
        this(e.getId() == null ? new ObjectId().toHexString() : e.getId().toHexString(), new UserDTO(e.getUser()), new OrganizationDTO(e.getOrg()), e.getStatus());
    }

    public RequestEntity toRequestEntity(){
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new RequestEntity(_id, user.toUserEntity(), org.toOrganizationEntity(), status);
    }
}
