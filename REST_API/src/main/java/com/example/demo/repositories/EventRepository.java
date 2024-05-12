package com.example.demo.repositories;

import com.example.demo.models.EventEntity;
import org.bson.types.ObjectId;

import java.util.List;

public interface EventRepository {
    ObjectId create(EventEntity eventEntity);

    List<EventEntity> search(String organizationId);
    List<EventEntity> findEventsOfUser(String userId);
}
