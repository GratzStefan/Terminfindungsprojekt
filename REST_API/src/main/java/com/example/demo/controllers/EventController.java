package com.example.demo.controllers;

import com.example.demo.dtos.EventDTO;
import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.services.EventService;
import org.bson.types.ObjectId;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/events")
public class EventController {
    private final static Logger LOGGER = LoggerFactory.getLogger(EventController.class);
    private final EventService eventService;

    public EventController(EventService eventService) {
        this.eventService = eventService;
    }

    @GetMapping("/search/{organizationid}")
    public ResponseEntity<List<EventDTO>> getUser(@PathVariable String organizationid) {
        List<EventDTO> eventDTOs = eventService.search(organizationid);

        if (eventDTOs.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(eventDTOs);
    }

    @GetMapping("/find/{userid}")
    public ResponseEntity<List<EventDTO>> getEventsOfUser(@PathVariable String userid) {
        List<EventDTO> eventDTOs = eventService.findEventsOfUser(userid);

        if (eventDTOs.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(eventDTOs);
    }

    @PostMapping("/add")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<String> postEvent(@RequestBody EventDTO EventDTO) {
        ObjectId id = eventService.create(EventDTO);
        if(id == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(id.toHexString());
    }
}

