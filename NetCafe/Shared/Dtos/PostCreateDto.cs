﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NetCafe.Shared.Dtos;

namespace NetCafe.Shared;

public class PostCreateDto
{
    public IFormFile? CoverImage { get; set; }

    [Required(ErrorMessage = "The post's title is required")]
    [MaxLength(150, ErrorMessage = "The title should not be greater than 150 characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "The post's content is required")]
    [MaxLength(150, ErrorMessage = "The content of the post should not exceed 15,000 characters")]
    public string? Content { get; set; }
    public Guid? SeriesId { get; set; }
    public List<TagDto>? Tags { get; set; }
}