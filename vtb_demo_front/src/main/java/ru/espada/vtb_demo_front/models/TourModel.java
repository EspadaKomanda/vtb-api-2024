package ru.espada.vtb_demo_front.models;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class TourModel {

    private int id;
    private String name;
    private String image;
    private List<String> images;
    private String description;
    private String address;
    private String shortDescription;
    private List<String> categories;
    private int price;
    private List<ReviewData> reviews;
    private int rating;
    private List<PayData> payVariants;

}
