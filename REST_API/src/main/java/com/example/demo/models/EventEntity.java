package com.example.demo.models;

import org.bson.types.ObjectId;

import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.List;
import java.util.Objects;

public class EventEntity {
    private ObjectId id;

    private String titel;
    private String description;
    private LocalDateTime datetime;
    private String organizationid;

    public EventEntity() {
    }

    public EventEntity(ObjectId id, String titel, String description, LocalDateTime datetime, String organizationid) {
        this.id = id;
        this.titel = titel;
        this.description = description;
        this.datetime = datetime;
        this.organizationid = organizationid;
    }

    public ObjectId getId() {
        return id;
    }

    public void setId(ObjectId id) {
        this.id = id;
    }

    public String getTitel() {
        return titel;
    }

    public void setTitel(String titel) {
        this.titel = titel;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public LocalDateTime getDateTime() {
        return datetime;
    }

    public void setDateTime(LocalDateTime datetime) {
        this.datetime = datetime;
    }

    public String getOrganizationid() {
        return organizationid;
    }

    public void setOrganizationid(String organizationid) {
        this.organizationid = organizationid;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        EventEntity that = (EventEntity) o;
        return Objects.equals(id, that.id) && Objects.equals(titel, that.titel) && Objects.equals(description, that.description) && Objects.equals(datetime, that.datetime) && Objects.equals(organizationid, that.organizationid);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, titel, description, datetime, organizationid);
    }
}
