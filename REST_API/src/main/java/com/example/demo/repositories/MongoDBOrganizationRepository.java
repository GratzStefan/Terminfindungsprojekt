package com.example.demo.repositories;

import com.example.demo.models.OrganizationEntity;
import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.Filters;
import com.mongodb.client.model.FindOneAndReplaceOptions;
import jakarta.annotation.PostConstruct;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.List;

import static com.mongodb.client.model.Filters.eq;
import static com.mongodb.client.model.ReturnDocument.AFTER;

@Repository
public class MongoDBOrganizationRepository implements OrganizationRepository{
    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<OrganizationEntity> organizationsCollection;

    public MongoDBOrganizationRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        organizationsCollection = client.getDatabase("Terminfindungsapp").getCollection("organizations", OrganizationEntity.class);
    }
    @Override
    public ObjectId create(OrganizationEntity organizationEntity) {
        Document doc = new Document();
        doc.append("name", organizationEntity.getName());
        if(organizationsCollection.find(doc).first() != null) return null;

        organizationEntity.setId(new ObjectId());
        organizationsCollection.insertOne(organizationEntity);

        return organizationEntity.getId();
    }

    @Override
    public ObjectId addUser(String userid, String organizationid, String adminid) {
        Document doc = new Document();
        doc.append("organizationid", organizationid);
        doc.append("users.userid", adminid);

        if(organizationsCollection.find(doc).first() != null) return null;



        organizationEntity.setId(new ObjectId());
        organizationsCollection.insertOne(organizationEntity);

        return organizationEntity.getId();
    }

    @Override
    public List<OrganizationEntity> search(String organizationName) {
        var regexFilter = Filters.regex("name", ".*?" + organizationName + ".*?", "i");

        return organizationsCollection.find(regexFilter).into(new ArrayList<>());
    }

    @Override
    public OrganizationEntity modify(OrganizationEntity organizationEntity) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        return organizationsCollection.findOneAndReplace(eq("_id", organizationEntity.getId()), organizationEntity, options);
    }

    @Override
    public long delete(String id) {
        return organizationsCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }
}
