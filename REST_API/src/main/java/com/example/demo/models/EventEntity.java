package com.example.demo.models;

import org.bson.types.ObjectId;
import org.springframework.cglib.core.Local;

import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.List;
import java.util.Objects;

public class EventEntity {
    private ObjectId id;

    private String titel;
    private String description;
    private LocalDateTime datetimestart;
    private LocalDateTime datetimeend;
    private String organizationid;

    public EventEntity() {
    }

    public EventEntity(ObjectId id, String titel, String description, LocalDateTime datetimestart, LocalDateTime datetimeend, String organizationid) {
        this.id = id;
        this.titel = titel;
        this.description = description;
        this.datetimestart = datetimestart;
        this.datetimeend = datetimeend;
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

    public LocalDateTime getDatetimestart() {
        return datetimestart;
    }

    public void setDatetimestart(LocalDateTime datetimestart) {
        this.datetimestart = datetimestart;
    }

    public LocalDateTime getDatetimeend() {
        return datetimeend;
    }

    public void setDatetimeend(LocalDateTime datetimeend) {
        this.datetimeend = datetimeend;
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
        return Objects.equals(id, that.id) && Objects.equals(titel, that.titel) && Objects.equals(description, that.description) && Objects.equals(datetimestart, that.datetimestart) && Objects.equals(datetimeend, that.datetimeend) && Objects.equals(organizationid, that.organizationid);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id, titel, description, datetimestart, datetimeend, organizationid);
    }
}
