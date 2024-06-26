package com.example.demo.services;

import com.example.demo.dtos.EventDTO;
import org.bson.types.ObjectId;

import java.util.List;

public interface EventService {
    ObjectId create(EventDTO EventDTO);
    List<EventDTO> search(String organizationid);
    List<EventDTO> findEventsOfUser(String userid);
}
