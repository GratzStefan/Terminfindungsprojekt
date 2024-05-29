package com.example.demo.repositories;

import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.OrganizationRole;
import com.example.demo.models.RequestEntity;
import com.example.demo.models.RequestStatus;
import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import jakarta.annotation.PostConstruct;
import org.bson.BsonValue;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

@Repository
public class MongoDBRequestRepository implements RequestRepository {
    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<RequestEntity> requestsCollection;
    private MongoCollection<OrganizationEntity> organizationsCollection;

    public MongoDBRequestRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        requestsCollection = client.getDatabase("Terminfindungsapp").getCollection("requests", RequestEntity.class);
        organizationsCollection = client.getDatabase("Terminfindungsapp").getCollection("organizations", OrganizationEntity.class);
    }

    @Override
    public RequestEntity create(RequestEntity requestEntity) {
        // Sets Status of Request to Waiting
        requestEntity.setStatus(RequestStatus.WAITING.ordinal());
        // Inserts Request into DB
        BsonValue id = requestsCollection.insertOne(requestEntity).getInsertedId();

        if(id==null) return null;

        return requestEntity;
    }

    @Override
    public List<RequestEntity> findToOrganization(String orgid) {
        // Filter-Criteria
        Document doc = new Document();
        doc.append("org._id", new ObjectId(orgid));
        doc.append("status", 0);
        // Finds all Request to an Organization
        return requestsCollection.find(doc).into(new ArrayList<>());
    }

    @Override
    public List<RequestEntity> findOfUser(String userid) {
        // Filter-Criteria
        Document doc = new Document();
        doc.append("user._id", new ObjectId(userid));

        // Finds all Request Sent by an User
        return requestsCollection.find(doc).into(new ArrayList<>());
    }

    @Override
    public long changeStatus(String adminid, RequestEntity requestEntity) {
        // Filter-Criteria
        Document doc = new Document();
        doc.append("_id", requestEntity.getOrg().getId());
        doc.append("userlist." + adminid, 0);

        // Checks if user has Right to Accept/Reject Request
        OrganizationEntity org = organizationsCollection.find(doc).first();
        if(org == null)
            return 0;

        Document filter = new Document("_id", requestEntity.getId());
        Document update = new Document("$set", new Document("status", requestEntity.getStatus()));

        // Accepts/Rejects Request of User To Join Organization
        long count = requestsCollection.updateOne(filter, update).getModifiedCount();

        // If Accepted To Organization, now we add him in Userlist of Organization
        if(count==1 && requestEntity.getStatus() == RequestStatus.ACCEPTED.ordinal()) {
            filter = new Document("_id", org.getId());

            // Add user to userlist
            HashMap<String, Integer> userlist = org.getUserlist();
            userlist.put(requestEntity.getUser().getId().toHexString(), OrganizationRole.User.ordinal());
            update = new Document("$set", new Document("userlist", userlist));
            // Updating in DB
            organizationsCollection.updateOne(filter, update);
        }

        return count;
    }
}
