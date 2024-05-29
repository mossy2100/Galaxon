-- Load all planets and dwarf planets, and all associated data records.
select *
from Astro.AstroObjects ao
left join Astro.Orbitals orb on ao.Id = orb.AstroObjectId
left join Astro.Physicals p on ao.Id = p.AstroObjectId
left join Astro.Rotationals r on ao.Id = r.AstroObjectId
left join Astro.Observationals obs on ao.Id = obs.AstroObjectId
left join Astro.Atmospheres atm on ao.Id = atm.AstroObjectId
where ao.Id in (select ObjectsId from Astro.AstroObjectGroups g join Astro.AstroObjectGroupRecordAstroObjectRecord aog on g.Id = aog.GroupsId where g.Name in ("Planet", "Dwarf planet"))
order by SemiMajorAxis_km 
