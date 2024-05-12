package com.example.demo.services;

import com.example.demo.dtos.EventDTO;
import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.repositories.EventRepository;
import com.example.demo.repositories.OrganizationRepository;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class EventServiceImpl implements EventService{
    private final EventRepository eventRepository;

    public EventServiceImpl(EventRepository eventRepository) {
        this.eventRepository = eventRepository;
    }

    @Override
    public ObjectId create(EventDTO EventDTO) {
        return eventRepository.create(EventDTO.toEventEntity());
    }

    @Override
    public List<EventDTO> search(String organizationid) {
        return eventRepository.search(organizationid).stream().map(EventDTO::new).toList();
    }

    @Override
    public List<EventDTO> findEventsOfUser(String userid) {
        return eventRepository.findEventsOfUser(userid).stream().map(EventDTO::new).toList();
    }
}
