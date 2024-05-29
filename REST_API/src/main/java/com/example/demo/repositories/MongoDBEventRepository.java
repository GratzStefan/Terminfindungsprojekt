package com.example.demo.repositories;

import com.example.demo.models.EventEntity;
import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.UserEntity;
import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.Projections;
import jakarta.annotation.PostConstruct;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

@Repository
public class MongoDBEventRepository implements EventRepository{
    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<EventEntity> eventsCollection;
    private MongoCollection<OrganizationEntity> organizationsCollection;

    public MongoDBEventRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        eventsCollection = client.getDatabase("Terminfindungsapp").getCollection("events", EventEntity.class);
        organizationsCollection = client.getDatabase("Terminfindungsapp").getCollection("organizations",OrganizationEntity.class);
    }

    @Override
    public ObjectId create(EventEntity eventEntity) {
        // Inserting Event into DB
        eventsCollection.insertOne(eventEntity);

        return eventEntity.getId();
    }

    @Override
    public List<EventEntity> search(String organizationId) {
        // Search-Criteria
        Document doc = new Document();
        doc.append("organizationid", organizationId);

        // Searching Events of Organization
        return eventsCollection.find(doc).into(new ArrayList<>());
    }

    @Override
    public List<EventEntity> findEventsOfUser(String userId) {
        // Search-Criteria
        Document filter = new Document();
        filter.append("userlist." + userId, new Document("$exists", true));

        // Finds all Organizations, where User is in
        List<String> usersOrgs = organizationsCollection.find(filter)
                .projection(new Document("_id", 1))
                .map(doc -> doc.getId().toHexString())
                .into(new ArrayList<>());

        // Filter-Criteria
        Document doc = new Document();
        doc.append("organizationid", new Document("$in", usersOrgs));

        // Finds all Events of Users Organizations
        return eventsCollection.find(doc).into(new ArrayList<>());
    }
}
