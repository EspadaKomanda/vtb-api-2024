package ru.espada.vtb_demo_front.models;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class ReviewData {

    private int userId;
    private String userName;
    private String userImage;
    private String review;
    private String date;
    private int rating;
    
}
