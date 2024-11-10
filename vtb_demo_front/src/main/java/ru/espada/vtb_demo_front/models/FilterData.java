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
public class FilterData {

    private String search;

    private String dateFrom;
    private String dateTo;

    private int priceFrom;
    private int priceTo;

    private int distanceFrom;
    private int distanceTo;

    private List<String> types;

    private int ratingFrom;

    private boolean credit;

}
