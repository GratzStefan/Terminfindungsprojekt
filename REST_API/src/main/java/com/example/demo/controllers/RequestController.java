package com.example.demo.controllers;

import com.example.demo.dtos.RequestDTO;
import com.example.demo.services.RequestService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

// Request-Controller

@RestController
@RequestMapping("/api/request")
public class RequestController {
    private final static Logger LOGGER = LoggerFactory.getLogger(UserController.class);
    private final RequestService requestService;

    public RequestController(RequestService requestService) {
        this.requestService = requestService;
    }

    @GetMapping("/findToOrganization/{orgid}")
    public ResponseEntity<List<RequestDTO>> getRequestsToOrganization(@PathVariable String orgid) {
        List<RequestDTO> dto = requestService.findToOrganization(orgid);
        if (dto == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(dto);
    }

    @GetMapping("/findOfUser/{userid}")
    public ResponseEntity<List<RequestDTO>> getRequestsOfUser(@PathVariable String userid) {
        List<RequestDTO> dto = requestService.findOfUser(userid);
        if (dto == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(dto);
    }

    @PostMapping("/send")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<RequestDTO> sendRequest(@RequestBody RequestDTO RequestDTO) {
        RequestDTO dto = requestService.create(RequestDTO);
        if (dto == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(dto);
    }

    @PutMapping("/changeStatus")
    public long changeStatus(@RequestParam String adminid, @RequestBody RequestDTO RequestDTO) {
        return requestService.changeStatus(adminid, RequestDTO);
    }

}
