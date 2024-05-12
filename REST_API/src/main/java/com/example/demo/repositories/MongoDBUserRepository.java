package com.example.demo.repositories;

import com.example.demo.dtos.UserDTO;
import com.example.demo.models.UserEntity;
import com.mongodb.ReadConcern;
import com.mongodb.ReadPreference;
import com.mongodb.TransactionOptions;
import com.mongodb.WriteConcern;
import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoCollection;
import com.mongodb.client.model.FindOneAndReplaceOptions;
import jakarta.annotation.PostConstruct;
import org.bson.Document;
import org.bson.types.ObjectId;
import org.springframework.security.crypto.scrypt.SCryptPasswordEncoder;
import org.springframework.stereotype.Repository;

import java.util.HashMap;
import java.util.Map;

import static com.mongodb.client.model.Filters.eq;
import static com.mongodb.client.model.ReturnDocument.AFTER;

@Repository
public class MongoDBUserRepository implements UserRepository {
    private static final TransactionOptions txnOptions = TransactionOptions.builder()
            .readPreference(ReadPreference.primary())
            .readConcern(ReadConcern.MAJORITY)
            .writeConcern(WriteConcern.MAJORITY)
            .build();
    private final MongoClient client;
    private MongoCollection<UserEntity> usersCollections;

    public MongoDBUserRepository(MongoClient mongoClient) {
        this.client = mongoClient;
    }

    @PostConstruct
    void init() {
        usersCollections = client.getDatabase("Terminfindungsapp").getCollection("users", UserEntity.class);
    }

    @Override
    public UserEntity register(UserEntity userEntity) {
        Document doc = new Document();
        doc.append("username", userEntity.getUsername());
        if(usersCollections.find(doc).first() != null) return null;

        userEntity.setId(new ObjectId());
        userEntity.setPassword(hashPassword(userEntity.getPassword()));
        usersCollections.insertOne(userEntity);

        return userEntity;
    }

    @Override
    public UserDTO login(String username, String password){
        Document doc = new Document();
        doc.append("username", username);

        UserEntity entity = usersCollections.find(doc).first();
        if(entity != null){
            SCryptPasswordEncoder encoder = new SCryptPasswordEncoder(16384, 8, 4, 32, 64);
            if(encoder.matches(password, entity.getPassword())){
                entity.setPassword(null);
                return new UserDTO(entity);
            }
        }

        return null;
    }

    @Override
    public long update(UserEntity userEntity) {
        Map<String, Object> changes = new HashMap<>();

        if(userEntity.getUsername() != null) {
            changes.put("username", userEntity.getUsername());
        }

        if(userEntity.getPassword() != null) {
            changes.put("password", hashPassword(userEntity.getPassword()));
        }

        if(userEntity.getFirstname() != null) {
            changes.put("firstname", userEntity.getFirstname());
        }

        if(userEntity.getLastname() != null) {
            changes.put("lastname", userEntity.getLastname());
        }

        Document filter = new Document("_id", userEntity.getId());
        Document update = new Document("$set", changes);

        return usersCollections.updateOne(filter, update).getModifiedCount();
    }

    @Override
    public long delete(String id) {
        return usersCollections.deleteOne(eq("_id", new ObjectId(id))).getDeletedCount();
    }

    private String hashPassword(String pw){
        SCryptPasswordEncoder encoder = new SCryptPasswordEncoder(16384, 8, 4, 32, 64);
        return encoder.encode(pw);
    }
}
