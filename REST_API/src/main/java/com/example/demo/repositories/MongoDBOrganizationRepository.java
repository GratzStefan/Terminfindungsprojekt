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
        Document doc = new Document();
        doc.append("name", organizationEntity.getName());
        if(organizationsCollection.find(doc).first() != null) return null;

        organizationEntity.setId(new ObjectId());
        organizationsCollection.insertOne(organizationEntity);

        return organizationEntity.getId();
    }

    @Override
    public int addUser(String userid, String organizationid, String adminid) {
        Document doc = new Document();
        doc.append("_id", new ObjectId(organizationid));
        doc.append("userlist." + adminid, 0);

        OrganizationEntity organization = organizationsCollection.find(doc).first();
        if(organization == null) return 404;

        organization.addUser(userid);

        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        if(organizationsCollection.findOneAndReplace(eq("_id", new ObjectId(organizationid)), organization, options) == null) return 500;

        return 200;
    }

    @Override
    public List<OrganizationEntity> search(String organizationName) {
        var regexFilter = Filters.regex("name", ".*?" + organizationName + ".*?", "i");

        return organizationsCollection.find(regexFilter).into(new ArrayList<>());
    }

    @Override
    public List<OrganizationEntity> searchOrganization(String userid) {
        Document doc = new Document();
        doc.append("userlist." + userid, 0);
        return organizationsCollection.find(doc).into(new ArrayList<>());
    }

    @Override
    public List<UserEntity> userListOfOrganization(String orgid) {
        Document doc = new Document();
        doc.append("_id", new ObjectId(orgid));

        //Document proj = new Document("userlist.", 0);

        OrganizationEntity dto = organizationsCollection.find(doc).first();
        if(dto==null){
            return null;
        }
        List<UserEntity> userList = new ArrayList<>();

        for(var userid : dto.getUserlist().entrySet()){
            Document user = new Document();
            user.append("_id", new ObjectId(userid.getKey()));

            Document projection = new Document("password", 0);
            projection.append("password", 0);

            UserEntity entity = usersCollection.find(user).projection(projection).first();
            if(entity != null){
                userList.add(entity);
            }
        }
        return userList;
    }

    @Override
    public OrganizationEntity modify(OrganizationEntity organizationEntity) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        return organizationsCollection.findOneAndReplace(eq("_id", organizationEntity.getId()), organizationEntity, options);
    }

    @Override
    public boolean promoteUser(String orgid, String userid, String adminid) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);

        Document doc = new Document();
        doc.append("_id", new ObjectId(orgid));
        doc.append("userlist." + adminid, 0);
        OrganizationEntity org = organizationsCollection.find(doc).first();
        if(org!=null){
            org.getUserlist().replace(userid, 0);
            return organizationsCollection.findOneAndReplace(eq("_id", new ObjectId(orgid)), org, options) != null;
        }

        return false;
    }

    @Override
    public long delete(String id) {
        return organizationsCollection.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }

    @Override
    public boolean removeUser(String userid, String orgid, String adminid) {
        FindOneAndReplaceOptions options = new FindOneAndReplaceOptions().returnDocument(AFTER);
        Document doc = new Document();
        doc.append("_id", new ObjectId(orgid));
        doc.append("userlist." + adminid, 0);
        OrganizationEntity org = organizationsCollection.find(doc).first();
        if(org!=null){
            org.getUserlist().remove(userid);
            return organizationsCollection.findOneAndReplace(eq("_id", new ObjectId(orgid)), org, options) != null;
        }

        return false;
    }
}
