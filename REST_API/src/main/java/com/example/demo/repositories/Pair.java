package com.example.demo.repositories;

public class Pair<T, U> {
    public final T username;
    public final U type;

    public Pair(T username, U type) {
        this.username = username;
        this.type = type;
    }

}
