package com.example.demo.repositories;

import com.example.demo.models.EventEntity;
import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import jakarta.annotation.PostConstruct;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;

@Repository
public class MongoDBEventRepository implements EventRepository{
    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<EventEntity> eventsCollection;

    public MongoDBEventRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        eventsCollection = client.getDatabase("Terminfindungsapp").getCollection("events", EventEntity.class);
    }

    @Override
    public ObjectId create(EventEntity eventEntity) {
        eventsCollection.insertOne(eventEntity);

        return eventEntity.getId();
    }

    @Override
    public List<EventEntity> search(String organizationId) {
        Document doc = new Document();
        doc.append("organizationid", organizationId);

        return eventsCollection.find(doc).into(new ArrayList<>());
    }

    /*
    @Override
    public OrganizationEntity modify(OrganizationEntity organizationEntity) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        return organizationsCollection.findOneAndReplace(eq("_id", organizationEntity.getId()), organizationEntity, options);
    }

    @Override
    public long delete(String id) {
        return organizationsCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }*/
}
