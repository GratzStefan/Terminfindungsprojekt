package com.example.demo;

import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

// CORS-Config (so Server can be accessed from different Port)

@Configuration
public class CorsConfig implements WebMvcConfigurer {
    @Override
    public void addCorsMappings(CorsRegistry registry) {
        registry.addMapping("/**")
                .allowedOrigins("*") // or specific origins
                .allowedMethods("GET", "POST", "PUT", "DELETE") // or specific methods
                .allowedHeaders("*"); // or specific headers
    }
}
