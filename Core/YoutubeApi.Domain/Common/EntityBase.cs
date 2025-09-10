﻿namespace YoutubeApi.Domain.Common;

public class EntityBase:IEntityBase
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsDeleted { get; set; } = false;

}
