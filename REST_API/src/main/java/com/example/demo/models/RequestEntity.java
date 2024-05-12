package com.example.demo.models;

import org.bson.types.ObjectId;

import java.util.Objects;

public class RequestEntity {
    private ObjectId id;
    private UserEntity user;
    private OrganizationEntity org;
    private Integer status;


    public RequestEntity() {
    }

    public RequestEntity(ObjectId id, UserEntity user, OrganizationEntity org, int status) {
        this.id = id;
        this.user = user;
        this.org = org;
        this.status = status;
    }

    public ObjectId getId() {
        return id;
    }

    public void setId(ObjectId id) {
        this.id = id;
    }

    public UserEntity getUser() {
        return user;
    }

    public void setUser(UserEntity user) {
        this.user = user;
    }

    public OrganizationEntity getOrg() {
        return org;
    }

    public void setOrg(OrganizationEntity org) {
        this.org = org;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        RequestEntity that = (RequestEntity) o;
        return Objects.equals(id, that.id) && Objects.equals(user, that.user) && Objects.equals(org, that.org) && status == that.status;
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, user, org, status);
    }
}
