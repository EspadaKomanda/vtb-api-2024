package ru.espada.vtb_demo_front.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.espada.vtb_demo_front.models.FilterData;
import ru.espada.vtb_demo_front.service.TourService;

@RestController
@RequestMapping("/tour-service")
public class TourController {

    @Autowired
    private TourService tourService;

    @PostMapping("/tours/{page}")
    public ResponseEntity<?> getTours(@RequestBody FilterData filterData, @PathVariable int page) {
        return ResponseEntity.ok(tourService.getTours(filterData, page));
    }

    @GetMapping("/tour/{id}")
    public ResponseEntity<?> getTour(@PathVariable int id) {
        return ResponseEntity.ok(tourService.getTour(id));
    }

    @GetMapping("/tour/{id}/favorite")
    public ResponseEntity<?> favorite(@PathVariable int id) {
        return ResponseEntity.ok(tourService.isFavorite(id));
    }

    @GetMapping("/favorites")
    public ResponseEntity<?> getFavorites() {
        return ResponseEntity.ok(tourService.getFavorites());
    }

}
