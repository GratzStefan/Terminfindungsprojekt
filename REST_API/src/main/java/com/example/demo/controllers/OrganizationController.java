package com.example.demo.controllers;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.services.OrganizationService;
import org.bson.types.ObjectId;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/organization")
public class OrganizationController {
    private final static Logger LOGGER = LoggerFactory.getLogger(UserController.class);
    private final OrganizationService organizationService;

    public OrganizationController(OrganizationService organizationService) {
        this.organizationService = organizationService;
    }

    @GetMapping("/search/{pattern}")
    public ResponseEntity<List<OrganizationDTO>> getUser(@PathVariable String pattern) {
        List<OrganizationDTO> organizationDTOS = organizationService.search(pattern);

        if (organizationDTOS.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(organizationDTOS);
    }

    @PostMapping("/create")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<String> postOrganization(@RequestBody OrganizationDTO OrganizationDTO) {
        ObjectId id = organizationService.create(OrganizationDTO);
        if(id == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(id.toHexString());
    }

    @PutMapping("/addUser")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<String> addUser(@RequestParam String userid, @RequestParam String organizationid, @RequestParam String adminid) {
        ObjectId id = organizationService.addUser(userid, organizationid, adminid);
        if(id == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(id.toHexString());
    }
    @PutMapping("/modify")
    public OrganizationDTO putOrganization(@RequestBody OrganizationDTO OrganizationDTO) {
        return organizationService.modify(OrganizationDTO);
    }

    @DeleteMapping("/delete/{id}")
    public Long deleteOrganization(@PathVariable String id) {
        return organizationService.delete(id);
    }
}