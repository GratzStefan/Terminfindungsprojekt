package com.example.demo.models;

import org.bson.types.ObjectId;

import java.util.HashMap;
import java.util.List;
import java.util.Objects;

public class OrganizationEntity {
    private ObjectId id;

    private String name;
    private String creatorid;
    private HashMap<String, Integer> userlist = new HashMap<String, Integer>();

    public OrganizationEntity() {
    }

    public OrganizationEntity(ObjectId id, String name) {
        this.id = id;
        this.name = name;
    }

    public OrganizationEntity(ObjectId id, String name, HashMap<String, Integer> userlist) {
        this.id = id;
        this.name = name;
        this.userlist = userlist;
    }

    public ObjectId getId() {
        return id;
    }

    public void setId(ObjectId id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void addUser(String userid){
        userlist.put(userid, OrganizationRole.User.ordinal());
    }
    public void setUserlist(HashMap<String, Integer> userlist) {
        this.userlist = userlist;
    }
    public HashMap<String, Integer> getUserlist() {
        return userlist;
    }

    public String getCreatorid() {
        return creatorid;
    }

    public void setCreatorid(String creatorid) {
        this.creatorid = creatorid;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        OrganizationEntity entity = (OrganizationEntity) o;
        return Objects.equals(id, entity.id) && Objects.equals(name, entity.name) && Objects.equals(creatorid, entity.creatorid) && Objects.equals(userlist, entity.userlist);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, name, creatorid, userlist);
    }
}
