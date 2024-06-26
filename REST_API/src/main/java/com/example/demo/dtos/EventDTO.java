package com.example.demo.dtos;

import com.example.demo.models.EventEntity;
import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.OrganizationRole;
import com.example.demo.models.UserEntity;
import com.fasterxml.jackson.annotation.JsonInclude;
import org.bson.types.ObjectId;

import java.time.LocalDateTime;

@JsonInclude(JsonInclude.Include.NON_NULL)
public record EventDTO (
        String id,
        String titel,
        String description,
        String datetimestart,
        String datetimeend,
        String organizationid) {

    public EventDTO(EventEntity e){
        this(e.getId() == null ? new ObjectId().toHexString() : e.getId().toHexString(), e.getTitel(), e.getDescription(), e.getDatetimestart().toString(), e.getDatetimeend().toString(), e.getOrganizationid());
    }

    public EventEntity toEventEntity(){
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);

        return new EventEntity(_id, titel, description, LocalDateTime.parse(datetimestart), LocalDateTime.parse(datetimeend), organizationid);
    }
}

