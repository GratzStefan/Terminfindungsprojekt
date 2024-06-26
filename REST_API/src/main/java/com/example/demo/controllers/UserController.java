package com.example.demo.controllers;

import com.example.demo.dtos.UserDTO;
import com.example.demo.services.UserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

// User-Controller

@RestController
@RequestMapping("/api/user")
public class UserController {
    private final static Logger LOGGER = LoggerFactory.getLogger(UserController.class);
    private final UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    @GetMapping("/login")
    public ResponseEntity<UserDTO> getUser( @RequestParam String username, @RequestParam String password) {
        UserDTO UserDTO = userService.login(username, password);
        if (UserDTO == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(UserDTO);
    }

    @PostMapping("/signup")
    @ResponseStatus(HttpStatus.CREATED)
    public ResponseEntity<UserDTO> postUser(@RequestBody UserDTO UserDTO) {
        UserDTO dto = userService.register(UserDTO);
        if (dto == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(dto);
    }
    @PutMapping("/modifyUser")
    public long putUser(@RequestBody UserDTO UserDTO) {
        return userService.update(UserDTO);
    }
}
