package ru.espada.vtb_demo_front.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.espada.vtb_demo_front.service.ServiceService;

@RestController
@RequestMapping("/service")
public class ServiceController {

    @Autowired
    private ServiceService serviceService;

    @GetMapping("/services/{page}")
    public ResponseEntity<?> getServices(@PathVariable int page) {
        return ResponseEntity.ok(serviceService.getServices(page));
    }

    @GetMapping("/services/recommend/{id}")
    public ResponseEntity<?> getRecomendServices(@PathVariable int id, @RequestParam int page) {
        return ResponseEntity.ok(serviceService.getRecommendedServices(id, page));
    }

    @GetMapping("/favorites")
    public ResponseEntity<?> getFavorites() {
        return ResponseEntity.ok(serviceService.getFavorites());
    }
}
