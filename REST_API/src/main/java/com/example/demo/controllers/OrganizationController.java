package com.example.demo.controllers;

import com.example.demo.dtos.OrganizationDTO;
import com.example.demo.dtos.UserDTO;
import com.example.demo.models.UserEntity;
import com.example.demo.repositories.Pair;
import com.example.demo.services.OrganizationService;
import org.bson.types.ObjectId;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.List;
    
@RestController
@RequestMapping("/api/organization")
public class OrganizationController {
    private final static Logger LOGGER = LoggerFactory.getLogger(OrganizationController.class);
    private final OrganizationService organizationService;

    public OrganizationController(OrganizationService organizationService) {
        this.organizationService = organizationService;
    }

    @GetMapping("/search/{pattern}")
    public ResponseEntity<List<OrganizationDTO>> searchOrganizations(@PathVariable String pattern) {
        List<OrganizationDTO> organizationDTOS = organizationService.search(pattern);

        if (organizationDTOS.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(organizationDTOS);
    }

    @GetMapping("/searchOrganizations/{pattern}")
    public ResponseEntity<List<OrganizationDTO>> getOrganizations(@PathVariable String pattern) {
        List<OrganizationDTO> organizations = organizationService.searchOrganizations(pattern);

        if (organizations.isEmpty()) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(organizations);
    }

    @GetMapping("/userListOrganization/{orgID}")
    public ResponseEntity<List<UserDTO>> getUserListOfOrganization(@PathVariable String orgID) {
        List<UserDTO> users = organizationService.userListOfOrganization(orgID);

        if (users == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(users);
    }

    @PostMapping("/create")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<String> postOrganization(@RequestBody OrganizationDTO OrganizationDTO) {
        System.out.println(OrganizationDTO.name());
        ObjectId id = organizationService.create(OrganizationDTO);
        if(id == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(id.toHexString());
    }

    @PutMapping("/addUser")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<String> addUser(@RequestParam String userid, @RequestParam String organizationid, @RequestParam String adminid) {
        if(userid.equals(adminid)) return ResponseEntity.status(HttpStatus.NOT_ACCEPTABLE).build();
        int id = organizationService.addUser(userid, organizationid, adminid);

        return ResponseEntity.status(HttpStatus.valueOf(id)).build();
    }

    @PutMapping("/modify")
    public OrganizationDTO putOrganization(@RequestBody OrganizationDTO OrganizationDTO) {
        return organizationService.modify(OrganizationDTO);
    }

    @PutMapping("/promote")
    public boolean promoteUser(@RequestParam String orgid, @RequestParam String userid, @RequestParam String adminid) {
        return organizationService.promoteUser(orgid, userid, adminid);
    }

    @DeleteMapping("/delete/{id}")
    public Long deleteOrganization(@PathVariable String id) {
        return organizationService.delete(id);
    }

    @DeleteMapping("/removeUser")
    public boolean deleteOrganization(@RequestParam String userid, @RequestParam String orgid, @RequestParam String adminid) {
        return organizationService.removeUser(userid, orgid, adminid);
    }
}
