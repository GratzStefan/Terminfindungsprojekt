package com.example.demo.repositories;

import com.example.demo.models.OrganizationEntity;
import com.example.demo.models.UserEntity;
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

import java.util.*;

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
    private MongoCollection<UserEntity> usersCollection;

    public MongoDBOrganizationRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        organizationsCollection = client.getDatabase("Terminfindungsapp").getCollection("organizations", OrganizationEntity.class);
        usersCollection = client.getDatabase("Terminfindungsapp").getCollection("users", UserEntity.class);
    }

    @Override
    public ObjectId create(OrganizationEntity organizationEntity) {
        // Filter-Criteria
        Document doc = new Document();
        doc.append("name", organizationEntity.getName());

        // Checks if OrganizationName already exists
        if(organizationsCollection.find(doc).first() != null) return null;

        // Sets new ID
        organizationEntity.setId(new ObjectId());
        // Inserts new Organization
        organizationsCollection.insertOne(organizationEntity);

        return organizationEntity.getId();
    }

    @Override
    public List<OrganizationEntity> search(String organizationName) {
        // Regex to filter after OrganizationName for Search
        var regexFilter = Filters.regex("name", ".*?" + organizationName + ".*?", "i");

        // Gets Organizations
        return organizationsCollection.find(regexFilter).limit(25).into(new ArrayList<>());
    }

    @Override
    public List<OrganizationEntity> searchOrganization(String userid) {
        // Filter-Criteria
        Document doc = new Document();
        doc.append("userlist." + userid, new Document("$exists", true));
        // Finds Organizations of User
        return organizationsCollection.find(doc).into(new ArrayList<>());
    }

    @Override
    public List<UserEntity> userListOfOrganization(String orgid) {
        // Filter-Criteria
        Document doc = new Document();
        doc.append("_id", new ObjectId(orgid));

        // Finds specific Organization
        OrganizationEntity dto = organizationsCollection.find(doc).first();
        if(dto==null){
            return null;
        }

        // Converts Hashmap into List of UserEntity
        List<UserEntity> userList = new ArrayList<>();

        for(var userid : dto.getUserlist().entrySet()){
            // Filter-Criteria
            Document user = new Document();
            user.append("_id", new ObjectId(userid.getKey()));

            // Projection
            Document projection = new Document("password", 0);
            projection.append("password", 0);

            // Finds all Userinformation
            UserEntity entity = usersCollection.find(user).projection(projection).first();
            if(entity != null){
                userList.add(entity);
            }
        }
        return userList;
    }

    @Override
    public boolean promoteUser(String orgid, String userid, String adminid) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);

        // Filter-Criteria
        Document doc = new Document();
        doc.append("_id", new ObjectId(orgid));
        doc.append("userlist." + adminid, 0);
        // Checks if User has Right to Promote user
        OrganizationEntity org = organizationsCollection.find(doc).first();
        if(org!=null){
            // Changes Rights of User
            org.getUserlist().replace(userid, 0);
            return organizationsCollection.findOneAndReplace(eq("_id", new ObjectId(orgid)), org, options) != null;
        }

        return false;
    }

    @Override
    public long delete(String id) {
        // Deletes Organization
        return organizationsCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }

    @Override
    public boolean removeUser(String userid, String orgid, String adminid) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        // Filter-Criteria
        Document doc = new Document();
        doc.append("_id", new ObjectId(orgid));
        doc.append("userlist." + adminid, 0);

        // Checks if User has Right to Remove User from Organization
        OrganizationEntity org = organizationsCollection.find(doc).first();
        if(org!=null){
            // Removes User from Organization
            org.getUserlist().remove(userid);
            return organizationsCollection.findOneAndReplace(eq("_id", new ObjectId(orgid)), org, options) != null;
        }

        return false;
    }
}
